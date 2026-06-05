
using MegaCrit.Sts2.Core.Entities.Players;
using Phrolova.PhrolovaCode.Cards;
using Phrolova.PhrolovaCode.Relics;

namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class PerformingPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;
        public override bool IsInstanced => false;

        // 累计伤害（公开属性，供谢幕读取）
        public decimal TotalDamageDealt { get; set; }

        // 动态变量：用于在能力栏显示累计伤害
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(0m, ValueProp.Move)
        };

        private decimal GetHecateDamage()
        {
            decimal baseDamage = 3m;
            if (Owner?.Player?.Relics.OfType<UnderworldGaze>().Any() == true)
                baseDamage += 5;
            var enhance = Owner?.Powers.OfType<HecateEnhancePower>().FirstOrDefault();
            if (enhance != null)
                baseDamage += enhance.Amount; // 每层+1
            return baseDamage;
        }

        public async Task ApplyNoteEffects(int red, int blue, int colorful)
        {
            if (Owner == null) return;

            TotalDamageDealt = 0;
            DynamicVars.Damage.BaseValue = 0;

            int strengthBonus = red + colorful;
            int dexterityBonus = blue + colorful;

            // 施加临时力量和敏捷（回合结束自动移除）
            if (strengthBonus > 0)
                await PowerCmd.Apply<PerformStrengthPower>(Owner, strengthBonus, Owner, null);
            if (dexterityBonus > 0)
                await PowerCmd.Apply<PerformDexterityPower>(Owner, dexterityBonus, Owner, null);

            if (colorful > 0 && Owner.CombatState != null)
            {
                var enemies = Owner.CombatState.GetOpponentsOf(Owner).ToList(); // ← 拷贝副本
                foreach (var enemy in enemies)
                {
                    await PowerCmd.Apply<VulnerablePower>(enemy, colorful, Owner, null);
                    await PowerCmd.Apply<WeakPower>(enemy, colorful, Owner, null);
                }
            }

            // ★ 施加演奏重放效果（根据红与黑的歌的层数）
            var redBlack = Owner.Powers.OfType<RedBlackSongPower>().FirstOrDefault();
            if (redBlack != null && redBlack.Amount > 0)
            {
                await PowerCmd.Apply<PerformEchoPower>(Owner, redBlack.Amount, Owner, null);
            }
            
            // 引爆所有敌人身上的迷梦
            if (Owner?.CombatState != null)
            {
                var enemies = Owner.CombatState.GetOpponentsOf(Owner).ToList();
                foreach (var enemy in enemies)
                {
                    var dreamMark = enemy.Powers.OfType<DreamMarkPower>().FirstOrDefault();
                    if (dreamMark != null && dreamMark.Amount > 0)
                    {
                        int stacks = dreamMark.Amount;
                        // 造成伤害
                        await CreatureCmd.Damage(
                            new ThrowingPlayerChoiceContext(),
                            enemy,
                            stacks,
                            ValueProp.Unblockable | ValueProp.Unpowered,
                            Owner
                        );
                        // 如果没有“穿越生命的要地”能力，则移除迷梦
                        if (!Owner.Powers.OfType<VitalCrossingPower>().Any())
                        {
                            await PowerCmd.Remove(dreamMark);
                        }
                    }
                }
            }
            
            // 遗物：撕碎的乐谱
            if (Owner?.Player?.Relics.OfType<TornScore>().Any() == true)
                await PowerCmd.Apply<ResonancePower>(Owner, 5, Owner, null);
        }

        //public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
       // {
            //if (target == Owner) return 0.5m;
           // return 1m;
       // }

        public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature dealer, DamageResult result, ValueProp props, Creature target, CardModel cardSource)
        {
            
            if (dealer != Owner) return;
            if (cardSource == null) return;
            if (cardSource.Owner?.Creature != Owner) return;
            if (!props.IsPoweredAttack()) return;

            // 累加实际造成的伤害
            TotalDamageDealt += result.UnblockedDamage;
            DynamicVars.Damage.BaseValue = TotalDamageDealt;// 更新动态变量

            var handPile = PileType.Hand.GetPile(Owner.Player);
            if (handPile != null)
            {
                foreach (var card in handPile.Cards.OfType<CurtainCall>())
                    card.DynamicVars.Damage.BaseValue = TotalDamageDealt;
            }

            // 叠加余响
            await PowerCmd.Apply<ResonancePower>(Owner, 1, Owner, null);

            // 赫卡忒协战：基础1次 + 额外协战能力层数
            int extraAttacks = Owner.Powers.OfType<HecateExtraAttackPower>().Sum(p => p.Amount);
            int totalAttacks = 1 + extraAttacks;

            var hecate = Owner.Player.Creature.Pets
                .FirstOrDefault(p => p.Monster is Hecate);
            if (hecate != null && hecate.IsAlive && target != null)
            {
                for (int i = 0; i < totalAttacks; i++)
                {
                    await CreatureCmd.Damage(
                        choiceContext,
                        target,
                        GetHecateDamage(),
                        ValueProp.Unpowered,
                        hecate,
                        cardSource
                    );
                }
            }
        }

        // 下回合开始时移除演奏，回到定音
        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (player.Creature != Owner) return;

            // 清空所有乐声
            foreach (var p in Owner.Powers.OfType<RedNotePower>().ToList()) await PowerCmd.Remove(p);
            foreach (var p in Owner.Powers.OfType<BlueNotePower>().ToList()) await PowerCmd.Remove(p);
            foreach (var p in Owner.Powers.OfType<ColorfulNotePower>().ToList()) await PowerCmd.Remove(p);
            // 移除演奏重放能力
            foreach (var p in Owner.Powers.OfType<PerformEchoPower>().ToList())
                await PowerCmd.Remove(p);
            
            await PowerCmd.Remove(this);
            await PowerCmd.Apply<TuningStatePower>(Owner, 1, Owner, null);
        }
    }
}