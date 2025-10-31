namespace Swachify.Application;

public static class DbConstants
{
    public readonly static string fn_service_booking_list = "SELECT * FROM fn_service_booking_list(tickt_id:=-1)";
    public readonly static string fn_service_booking_list_by_Userid = "SELECT * FROM fn_service_booking_list(custid:={0})";
    public readonly static string fn_service_booking_list_by_Empid = "SELECT * FROM fn_service_booking_list(empid:={0})";
    public readonly static string fn_service_booking_list_by_Userid_Empid = "SELECT * FROM fn_service_booking_list({0},{1})";
    public readonly static string fn_get_all_masters_data = "SELECT * FROM fn_get_all_masters_data()";

    public readonly static string fn_service_booking_get_list_by_booking_id = "SELECT * FROM fn_service_booking_list(tickt_id:={0})";
}