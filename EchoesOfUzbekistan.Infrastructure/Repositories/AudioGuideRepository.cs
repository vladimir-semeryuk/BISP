using EchoesOfUzbekistan.Domain.Guides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Infrastructure.Repositories;
internal class AudioGuideRepository : Repository<AudioGuide>
{
    public AudioGuideRepository(AppDbContext context) : base(context)
    {
    }
}
