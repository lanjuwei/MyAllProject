using Model;

namespace BaseSetting.Needs
{
    /// <summary>
    /// 个性化需求 满足不同的图书馆要求
    /// </summary>
    public partial class IndividualNeeds
    {
        private static IndividualNeeds _individualNeeds;
        public static IndividualNeeds Instance => _individualNeeds ?? (_individualNeeds = new IndividualNeeds());

        /// <summary>
        /// 通用变量
        /// </summary>
        public CommonVariables CommonVariables { get; set; } = new CommonVariables();
        /// <summary>
        /// 界面变量
        /// </summary>
        public PageVariables PageVariables { get; set; } = new PageVariables();

    }
}