using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;

namespace Phrolova.Monsters
{
    public sealed class Hecate : CustomPetModel
    {
        public Hecate() : base(true) { }
        public Hecate(bool visibleHp) : base(visibleHp) { }

        public static Vector2 MinOffset => new Vector2(180f, -200f);
        public static Vector2 MaxOffset => new Vector2(250f, -180f);

        public override int MinInitialHp => 1;
        public override int MaxInitialHp => 1;
        public override bool IsHealthBarVisible => Creature.IsAlive;
        public override bool HasDeathSfx => true;

        public override NCreatureVisuals CreateCustomVisuals()
        {
            string scenePath = "res://Phrolova/scenes/hecate_spine.tscn";
            if (ResourceLoader.Exists(scenePath))
            {
                var scene = GD.Load<PackedScene>(scenePath);
                if (scene != null)
                {
                    var instance = scene.Instantiate<Node2D>();
                    var wrapper = new NCreatureVisuals();
                    wrapper.Name = "HecateVisuals";

                    // Reparent children, preserving unique names and owner
                    while (instance.GetChildCount() > 0)
                    {
                        var child = instance.GetChild(0);
                        instance.RemoveChild(child);
                        wrapper.AddChild(child);
                        child.Owner = wrapper;
                    }

                    return wrapper;
                }
            }

            string imagePath = "res://Phrolova/images/monsters/hecate.png";
            if (ResourceLoader.Exists(imagePath))
                return NodeFactory<NCreatureVisuals>.CreateFromResource(imagePath);
            return null;
        }

        public override CreatureAnimator SetupCustomAnimationStates(MegaSprite controller)
        {
            AnimState idle = new AnimState("Stand01", true);
            AnimState cast = new AnimState("Skill01_start", false);
            AnimState attack = new AnimState("Attack01", false);
            AnimState hurt = new AnimState("hurt", false);
            AnimState die = new AnimState("Break01", false);
            AnimState deadLoop = new AnimState("Break01", true);
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
