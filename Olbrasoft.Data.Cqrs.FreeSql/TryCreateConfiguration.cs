namespace Olbrasoft.Data.Cqrs.FreeSql;

/// <summary>
/// Method used to try resolve configuration.
/// </summary>
/// <param name="exactType">A exactly type of configuration like IConfiguration?</param>
/// <param name="configuration">exactly type configuration as IConfiguration or Null</param>
/// <returns>If the connection was established it returns true otherwise it returns false.</returns>
public delegate bool TryCreateConfiguration(Type exactType, out IConfiguration? configuration);