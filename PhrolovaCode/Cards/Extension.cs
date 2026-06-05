
using MegaCrit.Sts2.Core.CardSelection;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Extension : PhrolovaCard
    {
        public Extension() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(9m, ValueProp.Move)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            // 从抽牌堆选一张牌放到牌堆顶
            var drawPile = PileType.Draw.GetPile(Owner);
            if (drawPile.Cards.Count == 0) return;

            var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
            var card = (await CardSelectCmd.FromSimpleGrid(choiceContext, drawPile.Cards, Owner, prefs)).FirstOrDefault();
            if (card != null)
            {
                await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m); // 9 → 12
        }
    }
}