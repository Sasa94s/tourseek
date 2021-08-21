namespace tourseek_backend.domain.Entities.Base
{
    public interface IBaseIdEntity<TId> : IBaseEntity where TId : struct
    {
        public TId Id { get; set; }
    }
}