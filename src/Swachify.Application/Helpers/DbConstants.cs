namespace Swachify.Application;

public static class DbConstants
{
    public readonly static string fn_service_booking_list = "SELECT * FROM fn_service_booking_list(tickt_id:=-1,p_limit:={0}, p_offset:={1})";
    public readonly static string fn_service_booking_list_by_Userid = "SELECT * FROM fn_service_booking_list(custid:={0},p_limit:={1}, p_offset:={2})";
    public readonly static string fn_service_booking_list_by_Empid = "SELECT * FROM fn_service_booking_list(empid:={0},p_limit:={1}, p_offset:={2})";
    public readonly static string fn_service_booking_list_by_Userid_Empid = "SELECT * FROM fn_service_booking_list({0},{1})";
    public readonly static string fn_get_all_masters_data = "SELECT * FROM fn_get_all_masters_data()";

    public readonly static string fn_service_booking_get_list_by_booking_id = "SELECT * FROM fn_service_booking_list(tickt_id:={0})";
    public readonly static string fn_create_master_departments = "SELECT * FROM fn_create_master_departments(p_department_name := '{department_name}',p_service_name := '{service_name}',p_service_type := '{service_type}',p_price := {price})";
    public readonly static string fn_update_master_departments = "SELECT * FROM fn_create_master_departments(p_department_id:='{department_id}',p_department_name := '{department_name}',p_service_id:='{service_id}',p_service_name := '{service_name}',p_service_type_id:='{service_type_id}',p_service_type := '{service_type}',p_price := {price});";


}