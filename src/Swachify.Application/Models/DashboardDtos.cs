using Org.BouncyCastle.Pqc.Crypto.Lms;
using Swachify.Application.DTOs;

namespace Swachify.Application;

public class DashboardDtos
{
    public int? servicesount { get; set; }
    public int? pendingActivebookingcount { get; set; }

    public int? inprogressopenticketscount { get; set; }
    public int? availableEmployeeCount { get; set; }
    public List<AllBookingsDtos> Allbookings { get; set; }
}