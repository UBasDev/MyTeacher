using CoreService.Application.MQContracts.Commands.Abstracts;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfilePicture.WORKER.Consumers
{
    public class CreateProfilePictureMQConsumer : IConsumer<ICreateProfilePictureMQCommand>
    {
        public async Task Consume(ConsumeContext<ICreateProfilePictureMQCommand> context)
        {
            Console.WriteLine("----- Started consuming -----");
            var messageFromQueue = context.Message;
            Console.WriteLine("----- End consuming -----");
        }
    }
}
