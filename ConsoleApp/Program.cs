using ConsoleApp;
using Entify.Transformations;
using System.Data;
using System.Data.SqlClient;

string Connection = "Data Source=srvbaqueo.cak6bmmeobfq.us-east-2.rds.amazonaws.com;Initial Catalog=OMINCA;Persist Security Info=False;User ID=admin;Password=ServerBaqueo*2021";



try
{
    Minerals mineral = new()
    {
        OP = ProcedureOption.SELECT.ToString()
    };

    SqlParameter[] parameters = DataTransformations.EntityToParameters(mineral);

    using SqlConnection dbConnection = new(Connection);

    dbConnection.Open();
    Console.WriteLine(dbConnection.State.ToString());
    using SqlCommand command = new("SP_TBL_MINERALS", dbConnection) { CommandType = CommandType.StoredProcedure };
    if (parameters != null)
        foreach (SqlParameter parameter in parameters)
            command.Parameters.Add(parameter).Value = parameter.Value;

    DataTable dt = new();
    SqlDataAdapter da = new(command);

    da.Fill(dt);

    var result = DataTransformations.DataTableToList<Minerals>(dt);
    result.ForEach(mineral => Console.WriteLine(string.Join(" ", mineral.ID.ToString(), mineral.Mineral, mineral.CREATED.ToString())));
    dbConnection.Open();

}
catch (Exception e)
{
    Console.WriteLine(e.Message);   
}



Console.WriteLine("Hello, World!");
