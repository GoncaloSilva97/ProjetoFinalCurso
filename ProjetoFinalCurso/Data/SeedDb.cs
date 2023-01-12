using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using ProjetoFinalCurso.Data.Entities;
using ProjetoFinalCurso.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Net.WebRequestMethods;

namespace ProjetoFinalCurso.Data
{
    public class SeedDb
    {
        private readonly DataContext _contex;
        private readonly IUserHelper _userHelper;
        private Random _random;

        public SeedDb(DataContext contex, IUserHelper userHelper)
        {
            _contex = contex;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _contex.Database.MigrateAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Employee");
            await _userHelper.CheckRoleAsync("Customer");

            //Criação Admin

            var user = await _userHelper.GetUserbyEmailAsync("ngoncalorsilvabusiness@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Nuno",
                    LastName = "Silva",
                    Email = "ngoncalorsilvabusiness@gmail.com",
                    UserName = "ngoncalorsilvabusiness@gmail.com",
                    PhoneNumber = "212344555"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");

            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            //Criação Employee

            var user3 = await _userHelper.GetUserbyEmailAsync("joaoricardo@yopmail.com");
            if (user3 == null)
            {
                user3 = new User
                {
                    FirstName = "Luis",
                    LastName = "Souza",
                    Email = "joaoricardo@yopmail.com",
                    UserName = "joaoricardo@yopmail.com",
                    PhoneNumber = "212344555"
                };

                var result = await _userHelper.AddUserAsync(user3, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user3, "Employee");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user3);
                await _userHelper.ConfirmEmailAsync(user3, token);
            }
             
            var isInRole3 = await _userHelper.IsUserInRoleAsync(user3, "Employee");

            if (!isInRole3)
            {
                await _userHelper.AddUserToRoleAsync(user3, "Employee");
            }


            //Criação customer

            var user2 = await _userHelper.GetUserbyEmailAsync("luissouza@yopmail.com");
            if (user2 == null)
            {
                user2 = new User
                {
                    FirstName = "Joao",
                    LastName = "Ricardo",
                    Email = "luissouza@yopmail.com",
                    UserName = "luissouza@yopmail.com",
                    PhoneNumber = "212344555"
                };

                var result = await _userHelper.AddUserAsync(user2, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user2, "Customer");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user2);
                await _userHelper.ConfirmEmailAsync(user2, token);
            }

            var isInRole2 = await _userHelper.IsUserInRoleAsync(user2, "Customer");

            if (!isInRole2)
            {
                await _userHelper.AddUserToRoleAsync(user2, "Customer");
            }

            if (!_contex.PaymentMethods.Any())
            {
                //Payment Methods

                PaymentMethod paymentMethod1 = new PaymentMethod
                {
                    Name = "Bank Transfer",
                    Info = "0000000000",
                    User = user

                };

                _contex.PaymentMethods.Add(paymentMethod1);

                PaymentMethod paymentMethod2 = new PaymentMethod
                {
                    Name = "MBWay",   
                    Info = "96666666",
                    User = user

                };

                _contex.PaymentMethods.Add(paymentMethod2);

                //Establisments

                Establishment establishment1 = new Establishment
                {
                    Name = "RCA Club",
                    Adress = "R. João Saraiva 18, 1700-249 Lisboa",
                    City = "Lisbon",
                    ImageId = new Guid("db8c0d83-1ede-435a-9e7b-bb2d7a42c002"),
                    //Transfer = "0000000000",
                    //MBWay = "96666666" ,
                    User = user

                }; 

                _contex.Establishments.Add(establishment1);

                Establishment establishment2 = new Establishment
                {
                    Name = "Altice Arena",
                    Adress = "Rossio dos Olivais, 1990-231 Lisboa",
                    City = "Lisbon",
                    ImageId = new Guid("d2acc0d5-80eb-4194-bfd0-95eb696e320f"),
                    //Transfer = "1111111111",
                    //MBWay = "922222222",
                    User = user3

                };

                _contex.Establishments.Add(establishment2);

                

                

                //Bands for Concert 1
                

                Band band1 = AddBand("FLESHGOD APOCALYPSE",

                    "Francesco Paoli – lead vocals, rhythm guitar ," +
                    " Paolo Rossi – bass, clean vocals," +
                    " Francesco Ferrini – piano, string arrangements, orchestral effects," +
                    "Veronica Bordacchini – operatic vocals, " +
                    "Fabio Bartoletti – lead guitar, " +
                    "Eugene Ryabchenko – drums.",

                    "Fleshgod Apocalypse is an Italian symphonic death metal band. " +
                    "Formed in 2007, the group resides in Perugia and are currently signed to Nuclear Blast. " +
                    "They have released five full-length albums.",
                    "https://open.spotify.com/artist/5ctFffJBdJe8PZL7W7NeML"
                    //"https://www.youtube.com/FleshgodApocalypse," +
                    /*"https://www.instagram.com/fleshgodofficial/?hl=en"*/, "a5910557-6aaf-4bdd-a325-675de8d7c22e",  user, "Symphonic Metal");

                _contex.Bands.Add(band1);

                

                Band band2 = AddBand("Omnium Gatherum",

                    "Markus Vanhala – lead guitar ," +
                    "Aapo Koivisto – keyboards, backing vocals," +
                    "Jukka Pelkonen – lead vocals," +
                    "Mikko Kivistö – bass," +
                    "Atte Pesonen – drums," +
                    "Nick Cordle – rhythm guitar",

                    "Omnium Gatherum is a six-piece melodic death metal band from Finland, " +
                    "founded in the autumn of 1996. Although the band mainly follows the path of the melodic death metal genre," +
                    " much of their work shows strong influences from progressive metal, especially their later albums. " +
                    "They have released five full-length albums.",
                    "https://open.spotify.com/artist/52xuvlUvnxqH0xzxGPKXSu "
                    //", https://www.youtube.com/watch?v=v841VlNzg7s,https://www.instagram.com/omniumgatherumofficial/?hl=en" 
                    , "9515dcc1-ae85-4d5a-ad54-4803e31a64c7", user, "Symphonic Metal");
                _contex.Bands.Add(band2);

                //Bands for Concert 2

                

                Band band3 = AddBand("Bring Me the Horizon",

                    "Oliver Sykes – lead vocals , keyboards, programming ," +
                    "Matt Kean – bass, " +
                    "Lee Malia – lead guitar, rhythm guitar," +
                    "Matt Nicholls – drums," +
                    "Jordan Fish – keyboards, programming, drum pad, percussion, backing vocals",

                    "Bring Me the Horizon (often abbreviated as BMTH) are a British rock band formed in Sheffield in 2004. " +
                    "The group consists of lead vocalist Oliver Sykes, guitarist Lee Malia, bassist Matt Kean, drummer Matt Nicholls and " +
                    "keyboardist Jordan Fish. They are signed to RCA Records globally and Columbia Records exclusively in the United States.",
                    "https://open.spotify.com/artist/1Ffb6ejR6Fe5IamqA5oRUF?autoplay=true " 
                    //", https://www.youtube.com/channel/UCAayZDDj3uom0QpSJiwLoUw?feature=gws_kp_artist&feature=gws_kp_artist," +
                   /* "https://www.instagram.com/bringmethehorizon/?hl=en"*/, "a99f477f-d2e4-4cd9-9052-79ee568fedc8",  user, "Metalcore");

                _contex.Bands.Add(band3);

               

                Band band4 = AddBand("A Day to Remember",

                    "Jeremy McKinnon – lead vocals," +
                    "Neil Westfall – rhythm guitar, backing vocals," +
                    "Alex Shelnutt – drums," +
                    "Kevin Skaff – lead guitar, backing vocals",

                    "A Day to Remember (often abbreviated ADTR, and previously known as End of an Era) is an American rock band from Ocala, Florida," +
                    " founded in 2003 by guitarist Tom Denney and drummer Bobby Scruggs. They are known for their amalgamation of metalcore and pop punk.",
                    "https://open.spotify.com/artist/4NiJW4q9ichVqL1aUsgGAN " 
                    //",https://www.youtube.com/channel/UCjj9ewTJYkVjuXBVrSlpMug?feature=gws_kp_artist&feature=gws_kp_artist," +
                    /*"https://www.instagram.com/adtr/?hl=en"*/ , "ef79061e-a58a-4bdd-b09c-9c52d3463a05", user, "Alternative Metal/PopPunk");

                _contex.Bands.Add(band4);

                //TicketType ticketType1 = new TicketType{Name = "Relvado",User = user};

                //_contex.TicketTypes.Add(ticketType1);

                TicketType ticketType1 = new TicketType { Name = "Standing Arena", User = user };

                _contex.TicketTypes.Add(ticketType1);

                TicketType ticketType2 = new TicketType { Name = "Seated", User = user };

                _contex.TicketTypes.Add(ticketType2);

                TicketType ticketType3 = new TicketType { Name = "Golden Circle", User = user };

                _contex.TicketTypes.Add(ticketType3);

                //1 - Standing Arena
                //2 - Standing Arena + Seated
                //3 - Standing Arena + Golden Circle 
                //4 - Standing Arena + Seated + Golden Circle 

                //Concerts

                Concert concert1 = new Concert
                {
                    Establishmento = establishment1,
                    Title = "Motocultor Across Europe Tour 2023 - Lisboa",
                    Description = "\"Motocultor Festival Across Europe tour 2023\" é assim designada a nova digressão da lendária banda italiana de " +
                    "death metal orquestral FLESHGOD APOCALYPSE que conta também com os finlandeses Omnium Gatherum.\r\nUma digressão notoriamente " +
                    "patrocinada e apoiada pelo MOTOCULTOR FESTIVAL Open Air de França.",
                    Day = new DateTime(2023, 01, 25),
                    ImageId = new Guid("e756e3b8-853d-434c-bafa-6905c3535b72"),
                    //TicketTypes = 1,
                    //Stock = 200,
                    User = user

                };

                _contex.Concerts.Add(concert1);

                Concert concert2 = new Concert
                {
                    Establishmento = establishment2,
                    Title = "BRING ME THE HORIZON",
                    Description = "Os BRING ME THE HORIZON vêm acompanhados pelos convidados especiais A DAY TO REMEMBER - que atuam pela primeira vez" +
                    " no nosso país -, e ainda pelos POORSTACY. Os bilhetes custam entre 35€ e 45€. ",
                    Day = new DateTime(2023, 02, 15),
                    ImageId = new Guid("478cf73d-b252-4c12-810e-8a07e4d8920e"),
                    //TicketTypes = 4,
                    //Stock = 3000,
                    User = user3

                };

                _contex.Concerts.Add(concert2);

                //ConcertBand concertBand1 = AddConcertBand(band1, concert1, user);

                //ConcertBand concertBand2 = AddConcertBand(band2, concert1, user);

                //ConcertBand concertBand3 = AddConcertBand(band3, concert2, user);

                //ConcertBand concertBand4 = AddConcertBand(band4, concert2, user);

                //Ticket Types





                //Tickets

                AddTicket(concert1, ticketType1, 25, 200, user);
                AddTicket(concert2, ticketType1, 35, 8000, user3);
                AddTicket(concert2, ticketType2, 40, 2050, user3);
                AddTicket(concert2, ticketType3, 50, 2000, user3);

                await _contex.SaveChangesAsync();
            }


        }

        private Band AddBand(string name,string members, string description, string links,string guid,User user, string genre)
        {
            
            Band band1 = new Band
            {
                Name = name,
                Members = members,
                Genre = genre,
                Description = description,
                Links = links,
                ImageId = new Guid(guid),
                User = user

            };

            _contex.Bands.Add(band1);

            return band1;
        }

        //private ConcertBand AddConcertBand(Band band,Concert concert, User user)
        //{
        //    ConcertBand concertBand1= new ConcertBand
        //    {
        //        Banda = band,
        //        Concerto = concert,
        //        User = user

        //    };

        //    _contex.ConcertBands.Add(concertBand1);

        //    return concertBand1;
        //}

        private Ticket AddTicket( Concert concert,TicketType type, decimal price,int stock, User user)
        {
            Ticket ticket1 = new Ticket
            {
                Code = concert.Title + " " + type.Name,
                Concerto = concert,
                Type = type,
                Price = price,
                Stock = stock,
                User = user

            };

            

            _contex.Tickets.Add(ticket1);

            return ticket1;
        }

    }
}
