nuget pack src/ScopedUnitOfWork.Interfaces/ScopedUnitOfWork.Interfaces.csproj -Prop Configuration=Release -IncludeReferencedProjects

nuget pack src/ScopedUnitOfWork.EF.Core/ScopedUnitOfWork.EF.Core.csproj -Prop Configuration=Release -IncludeReferencedProjects

nuget pack src/ScopedUnitOfWork.EF6/ScopedUnitOfWork.EF6.csproj -Prop Configuration=Release -IncludeReferencedProjects
