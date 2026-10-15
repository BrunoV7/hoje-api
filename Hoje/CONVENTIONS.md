# Conventions
- Money: `Money` (long cents, BRL). Never double. Rounding from decimal: <your choice>.
- Business dates: DateOnly / SQL `date`. Real moments: DateTimeOffset UTC.
- IDs: Guid v7 (Guid.CreateVersion7()).
- Domain has no EF/Identity references. EF config via IEntityTypeConfiguration in Infrastructure.
- Secrets: user-secrets locally. Never in appsettings or git.