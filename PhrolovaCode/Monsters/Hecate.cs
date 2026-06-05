
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;


namespace Phrolova.Monsters
{
    public sealed class Hecate : CustomMonsterModel
    {
        public static Vector2 MinOffset => new Vector2(180f, -200f); 
        public static Vector2 MaxOffset => new Vector2(250f, -180f);
        
        public override int MinInitialHp => 1;
        public override int MaxInitialHp => 1;
        // 1. 显示血条，规则和奥斯提一模一样
        public override bool IsHealthBarVisible => Creature.IsAlive;

        // 2. 显示死亡音效的标记
        public override bool HasDeathSfx => true;

        // 3. 你的占位图片逻辑（保留）
        public override NCreatureVisuals CreateCustomVisuals()
        {
            string imagePath = "res://Phrolova/images/monsters/hecate.png";
            if (ResourceLoader.Exists(imagePath))
            {
                return NodeFactory<NCreatureVisuals>.CreateFromResource(imagePath);
            }
            return null;
        }

        // 4. 动画状态（完全照搬奥斯提的配置）
        public override CreatureAnimator SetupCustomAnimationStates(MegaSprite controller)
        {
            AnimState idle = new AnimState("idle_loop", true);
            AnimState cast = new AnimState("cast", false);
            AnimState attack = new AnimState("attack", false);
            AnimState hurt = new AnimState("hurt", false);
            AnimState die = new AnimState("die", false);
            AnimState deadLoop = new AnimState("dead_loop", true);
            AnimState revive = new AnimState("revive", false);

            idle.AddBranch("Hit", hurt, null);
            cast.NextState = idle; cast.AddBranch("Hit", hurt, null);
            attack.NextState = idle; attack.AddBranch("Hit", hurt, null);
            hurt.NextState = idle; hurt.AddBranch("Hit", hurt, null);
            die.NextState = deadLoop;
            revive.NextState = idle;

            CreatureAnimator animator = new CreatureAnimator(idle, controller);
            animator.AddAnyState("Attack", attack, null);
            animator.AddAnyState("Cast", cast, null);
            animator.AddAnyState("Dead", die, null);
            animator.AddAnyState("Revive", revive, null);
            return animator;
        }

        // 5. 不会主动攻击（NOTHING_MOVE）
        protected override MonsterMoveStateMachine GenerateMoveStateMachine()
        {
            var nothing = new MoveState("NOTHING_MOVE",
                (IReadOnlyList<Creature> _) => Task.CompletedTask,
                Array.Empty<AbstractIntent>());
            nothing.FollowUpState = nothing;
            return new MonsterMoveStateMachine(new List<MonsterState> { nothing }, nothing);
        }

        public override void BeforeRemovedFromRoom() { }
    }
}