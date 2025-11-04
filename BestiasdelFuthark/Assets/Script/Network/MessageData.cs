using System;

[Serializable]
public class MessageData
{
    // Tipo de mensaje (ej: "build_room", "update_health", "end_turn", ...)
    public string type = "";

    // Payload genérico en JSON o texto (ej: lista de cartas serializadas)
    public string payload = "";

    // Alias/compatibilidad con código antiguo que usaba "content"
    public string content = "";

    // Constructor vacío necesario para JsonUtility y para object initializer (new MessageData { ... })
    public MessageData() { }

    // Constructor de conveniencia
    public MessageData(string type, string payload)
    {
        this.type = type;
        this.payload = payload;
        this.content = payload; // mantener content sincronizado si se usa ese constructor
    }
}
