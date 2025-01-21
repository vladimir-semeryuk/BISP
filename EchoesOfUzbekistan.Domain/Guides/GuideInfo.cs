using EchoesOfUzbekistan.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Guides;
public record GuideInfo(string? value)
{
    public static GuideInfo Create(string description)
    {
        if (description.Length > 5000)
            throw new ArgumentException("The guide's description cannot exceed 5000 characters.");

        return new GuideInfo(description);
    }
};
