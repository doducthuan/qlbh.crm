namespace QLBH_Dion.Util.Entities
{
    public abstract class EntityBase<TKey> : IEntityBase<TKey>
    {
        public TKey Id { get; set; }
        public int Active { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
