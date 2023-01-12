namespace ProjetoFinalCurso.Data.Entities
{
    public class PaymentMethod : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Info { get; set; }

        public User User { get; set; }
    }
}
