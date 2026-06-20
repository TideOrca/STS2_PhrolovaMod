
using Phrolova.PhrolovaCode.Powers;


namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class RedBlackSong : PhrolovaCard
    {
        public RedBlackSong() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self) { }
        
        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Ethereal };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 施加“红与黑的歌”能力
            await PowerCmd.Apply<RedBlackSongPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, 1, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Ethereal);
        }
    }
}