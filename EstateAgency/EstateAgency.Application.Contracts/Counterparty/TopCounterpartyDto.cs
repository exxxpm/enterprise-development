using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgency.Application.Contracts.Counterparty;
public record TopCounterpartyDto(
    CounterpartyGetDto Client,
    int ApplicationCount
);
