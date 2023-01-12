using ProjetoFinalCurso.Data.Entities;
using ProjetoFinalCurso.Models;
using System;

namespace ProjetoFinalCurso.Helpers
{
    public interface IConverterHelper
    {
        Establishment toEstablishment(EstablishmentViewModel model, Guid imageId, bool isNew);

        EstablishmentViewModel toEstablishmentViewModel(Establishment establishment);

        Band toBand(BandViewModel model, Guid imageId, bool isNew);

        BandViewModel toBandViewModel(Band band);

        Concert toConcert(ConcertViewModel model, Guid imageId, bool isNew);

        ConcertViewModel toConcertViewModel(Concert concert);

        Ticket toTicket(TicketViewModel model, Guid imageId, bool isNew);
        
        TicketViewModel toTicketViewModel(Ticket ticket);
    }
}
