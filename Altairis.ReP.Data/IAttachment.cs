namespace Altairis.ReP.Data;

public interface IAttachment {

    DateTime DateCreated { get; set; }

    string FileName { get; set; }

    long FileSize { get; set; }

    int Id { get; set; }

    string StoragePath { get; set; }

}