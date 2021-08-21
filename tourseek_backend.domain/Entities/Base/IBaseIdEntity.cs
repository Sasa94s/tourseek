namespace tourseek_backend.domain.Entities.Base
{
    public interface IBaseIdEntity<TId> where TId : struct
    {
        public TId Id { get; set; }
    }
}