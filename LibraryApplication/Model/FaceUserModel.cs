using SqlSugar;

namespace Model
{
    [SugarTable("FaceUser")]
    public class FaceUserModel
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        [SugarColumn(IsNullable  = true)]
        public string Name { get; set; }
        [SugarColumn(IsNullable = true)]
        public string FaceId { get; set; }
        [SugarColumn(ColumnName = "Image",IsNullable =true)]
        public string ImageData { get; set; }
        [SugarColumn(IsIgnore =true)]
        public string Describe { get; set; }
    }
}