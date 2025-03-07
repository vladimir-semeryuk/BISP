﻿using EchoesOfUzbekistan.Domain.Guides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Places;
public interface IPlaceRepository
{
    Task<Place?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Place>> GetAllAsync(CancellationToken cancellationToken);
    void Add(Place place);
    void Update(Place place);
    void Delete(Place place);
}
