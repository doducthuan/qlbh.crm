namespace QLBH_Dion.Util.Entities
{
    public abstract class EntityCommon<TKey> : EntityBase<TKey>, IEntityCommon
    {
        /// <summary>
        /// Tên 
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// Mô tả
        /// </summary>
        //public string? Description { get; set; }
    }
}
