namespace BaseSetting.Needs
{
    /// <summary>
    /// 个性化需求 满足不同的图书馆要求
    /// </summary>
    public partial class IndividualNeeds
    {
        private static IndividualNeeds _individualNeeds;
        public static IndividualNeeds Instance => _individualNeeds ?? (_individualNeeds = new IndividualNeeds());
         

    }
}