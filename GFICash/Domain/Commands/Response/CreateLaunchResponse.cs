using System;

namespace Domain.Commands.Response
{
    public class CreateLaunchResponse
    {
        public Guid Id { get; set; }
        public decimal Valor { get; set; }
        public DateTime Dt { get; set; }
        public string Tipo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}