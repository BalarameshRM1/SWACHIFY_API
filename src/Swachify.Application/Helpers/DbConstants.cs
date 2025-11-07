namespace Swachify.Application;

public static class DbConstants
{
    //public readonly static string fn_users_list = "select * from fn_users_list({userid},{roleid},{limit},{offset})";
    public readonly static string fn_users_list = "select * from fn_users_list({0},{1},{2},{3})";
      //SELECT * FROM fn_service_booking_list({tickt_id},{custid},{empid},{p_status_id},{limit},{offset})
    public readonly static string fn_service_booking_list = "SELECT * FROM fn_service_booking_list({0},{1},{2},{3},{4},{5})";
    public readonly static string fn_get_all_masters_data = "SELECT * FROM fn_get_all_masters_data()";
    public readonly static string fn_create_master_departments = "SELECT * FROM fn_create_master_departments(p_department_name := '{department_name}',p_service_name := '{service_name}',p_service_type := '{service_type}',p_price := {price})";
    public readonly static string fn_update_master_departments = "SELECT * FROM fn_create_master_departments(p_department_id:='{department_id}',p_department_name := '{department_name}',p_service_id:='{service_id}',p_service_name := '{service_name}',p_service_type_id:='{service_type_id}',p_service_type := '{service_type}',p_price := {price});";


}