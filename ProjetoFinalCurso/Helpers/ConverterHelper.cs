using ProjetoFinalCurso.Data.Entities;
using ProjetoFinalCurso.Models;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace ProjetoFinalCurso.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Establishment toEstablishment(EstablishmentViewModel model, Guid imageId, bool isNew)
        {
            return new Establishment
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Adress = model.Adress,
                City = model.City,
                ImageId = imageId,
                User = model.User
            };
        }

        public EstablishmentViewModel toEstablishmentViewModel(Establishment establishment)
        {
            return new EstablishmentViewModel
            {
                Id = establishment.Id,
                Name = establishment.Name,
                Adress = establishment.Adress,
                City = establishment.City,
                ImageId = establishment.ImageId,
                User = establishment.User
            };
        }

        public Band toBand(BandViewModel model, Guid imageId, bool isNew)
        {
            return new Band
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Members = model.Members,
                Description = model.Description,
                Links = model.Links,
                ImageId = imageId,
                Genre = model.Genre,
                User = model.User
            };
        }

        public BandViewModel toBandViewModel(Band band)
        {
            return new BandViewModel
            {
                Id = band.Id,
                Name = band.Name,
                Members = band.Members,
                Description = band.Description,
                Links = band.Links,
                ImageId = band.ImageId,
                Genre = band.Genre,
                User = band.User
            };
        }

        public Concert toConcert(ConcertViewModel model, Guid imageId, bool isNew)
        {
            return new Concert
            {
                Id = isNew ? 0 : model.Id,
                Establishmento = model.Establishmento,
                Title = model.Title,
                Description = model.Description,
                Day = model.Day,
                Bands = model.Bands,
                ImageId = imageId,
                User = model.User
            };
        }

        public ConcertViewModel toConcertViewModel(Concert concert)
        {
            return new ConcertViewModel
            {
                Id = concert.Id,
                Establishmento = concert.Establishmento,
                Title = concert.Title,
                Description = concert.Description,
                Day = concert.Day,
                Bands = concert.Bands,
                ImageId = concert.ImageId,
                User = concert.User
            };
        }

        public Ticket toTicket(TicketViewModel model, Guid imageId, bool isNew)
        {
            return new Ticket
            {
                Id = isNew ? 0 : model.Id,
                Code = model.Code,
                Concerto = model.Concerto,
                Price = model.Price,
                Stock = model.Stock,
                Type = model.Type,
                User = model.User
            };
        }

        public TicketViewModel toTicketViewModel(Ticket ticket)
        {
            return new TicketViewModel
            {
                Id = ticket.Id,
                Code = ticket.Code,
                Concerto = ticket.Concerto,
                Price = ticket.Price,
                Stock = ticket.Stock,
                Type = ticket.Type,
                User = ticket.User
            };
        }
    }
}
