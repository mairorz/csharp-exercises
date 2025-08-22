using WebApplicationUsers.Models;

namespace WebApplicationUsers.Services;

public class UserDataStore
{
    public List<User> Users { get; set; }

    public static UserDataStore Current { get; } = new UserDataStore();
    public UserDataStore()
    {
        Users = new List<User>()
        {
            new User(){
                Id = 1,
                Nombres = "Luis Fernando",
                Apellidos = "Gomez Rojas",
                Correo = "fernando@gmail.com",
                Telefono = "31124321",
                Username = "fercho"
            },
            new User(){
                Id = 2,
                Nombres = "María José",
                Apellidos = "Ramírez López",
                Correo = "mariajose.ramirez@example.com",
                Telefono = "44219876",
                Username = "majo23"
            },
            new User(){
                Id = 3,
                Nombres = "Carlos Andrés",
                Apellidos = "Pérez Molina",
                Correo = "carlosaperez@hotmail.com",
                Username = "carlitos"
            }
        };
    }
}
