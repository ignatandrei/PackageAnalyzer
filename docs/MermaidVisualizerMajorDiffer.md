
# Number of packages that differ in major version : 14


## 1 Microsoft.Bcl.AsyncInterfaces

```mermaid
---
title: Microsoft.Bcl.AsyncInterfaces
---
        flowchart TB
        subgraph packages
            Microsoft.Bcl.AsyncInterfaces

        end
        subgraph projects
NetTilt.GeneratorFromDB(((NetTilt.GeneratorFromDB)))
NetTilt.Context(((NetTilt.Context)))
NetTilt.Models(((NetTilt.Models)))
NetTilt.WebAPI(((NetTilt.WebAPI)))
NetTilt.Auth(((NetTilt.Auth)))
NetTilt.Logic(((NetTilt.Logic)))
NetTilt.Tests(((NetTilt.Tests)))
    subgraph versions
5.0.0>5.0.0]
6.0.0>6.0.0]
    end
Microsoft.Bcl.AsyncInterfaces-->5.0.0
5.0.0-->NetTilt.GeneratorFromDB
5.0.0-->NetTilt.Context
5.0.0-->NetTilt.Models
Microsoft.Bcl.AsyncInterfaces-->6.0.0
6.0.0-->NetTilt.WebAPI
6.0.0-->NetTilt.Auth
6.0.0-->NetTilt.Logic
6.0.0-->NetTilt.Tests

    end
```

## 2 Microsoft.Data.SqlClient

```mermaid
---
title: Microsoft.Data.SqlClient
---
        flowchart TB
        subgraph packages
            Microsoft.Data.SqlClient

        end
        subgraph projects
NetTilt.Context(((NetTilt.Context)))
NetTilt.Auth(((NetTilt.Auth)))
NetTilt.Logic(((NetTilt.Logic)))
NetTilt.Tests(((NetTilt.Tests)))
NetTilt.WebAPI(((NetTilt.WebAPI)))
    subgraph versions
2.1.4>2.1.4]
3.0.1>3.0.1]
    end
Microsoft.Data.SqlClient-->2.1.4
2.1.4-->NetTilt.Context
2.1.4-->NetTilt.Auth
2.1.4-->NetTilt.Logic
2.1.4-->NetTilt.Tests
Microsoft.Data.SqlClient-->3.0.1
3.0.1-->NetTilt.WebAPI

    end
```

## 3 Microsoft.Data.SqlClient.SNI.runtime

```mermaid
---
title: Microsoft.Data.SqlClient.SNI.runtime
---
        flowchart TB
        subgraph packages
            Microsoft.Data.SqlClient.SNI.runtime

        end
        subgraph projects
NetTilt.Context(((NetTilt.Context)))
NetTilt.Auth(((NetTilt.Auth)))
NetTilt.Logic(((NetTilt.Logic)))
NetTilt.Tests(((NetTilt.Tests)))
NetTilt.WebAPI(((NetTilt.WebAPI)))
    subgraph versions
2.1.1>2.1.1]
3.0.0>3.0.0]
    end
Microsoft.Data.SqlClient.SNI.runtime-->2.1.1
2.1.1-->NetTilt.Context
2.1.1-->NetTilt.Auth
2.1.1-->NetTilt.Logic
2.1.1-->NetTilt.Tests
Microsoft.Data.SqlClient.SNI.runtime-->3.0.0
3.0.0-->NetTilt.WebAPI

    end
```

## 4 Microsoft.Extensions.Configuration

```mermaid
---
title: Microsoft.Extensions.Configuration
---
        flowchart TB
        subgraph packages
            Microsoft.Extensions.Configuration

        end
        subgraph projects
NetTilt.Tests(((NetTilt.Tests)))
NetTilt.WebAPI(((NetTilt.WebAPI)))
    subgraph versions
7.0.0>7.0.0]
3.1.0>3.1.0]
    end
Microsoft.Extensions.Configuration-->7.0.0
7.0.0-->NetTilt.Tests
Microsoft.Extensions.Configuration-->3.1.0
3.1.0-->NetTilt.WebAPI

    end
```

## 5 Microsoft.Extensions.Configuration.Abstractions

```mermaid
---
title: Microsoft.Extensions.Configuration.Abstractions
---
        flowchart TB
        subgraph packages
            Microsoft.Extensions.Configuration.Abstractions

        end
        subgraph projects
NetTilt.Context(((NetTilt.Context)))
NetTilt.WebAPI(((NetTilt.WebAPI)))
NetTilt.Auth(((NetTilt.Auth)))
NetTilt.Logic(((NetTilt.Logic)))
NetTilt.Tests(((NetTilt.Tests)))
    subgraph versions
6.0.0>6.0.0]
7.0.0>7.0.0]
    end
Microsoft.Extensions.Configuration.Abstractions-->6.0.0
6.0.0-->NetTilt.Context
6.0.0-->NetTilt.WebAPI
6.0.0-->NetTilt.Auth
6.0.0-->NetTilt.Logic
Microsoft.Extensions.Configuration.Abstractions-->7.0.0
7.0.0-->NetTilt.Tests

    end
```

## 6 Microsoft.Extensions.DependencyInjection

```mermaid
---
title: Microsoft.Extensions.DependencyInjection
---
        flowchart TB
        subgraph packages
            Microsoft.Extensions.DependencyInjection

        end
        subgraph projects
NetTilt.Tests(((NetTilt.Tests)))
NetTilt.Context(((NetTilt.Context)))
NetTilt.Models(((NetTilt.Models)))
NetTilt.WebAPI(((NetTilt.WebAPI)))
NetTilt.Auth(((NetTilt.Auth)))
NetTilt.Logic(((NetTilt.Logic)))
    subgraph versions
7.0.0>7.0.0]
6.0.0>6.0.0]
    end
Microsoft.Extensions.DependencyInjection-->7.0.0
7.0.0-->NetTilt.Tests
Microsoft.Extensions.DependencyInjection-->6.0.0
6.0.0-->NetTilt.Context
6.0.0-->NetTilt.Models
6.0.0-->NetTilt.WebAPI
6.0.0-->NetTilt.Auth
6.0.0-->NetTilt.Logic

    end
```

## 7 Microsoft.Extensions.DependencyInjection.Abstractions

```mermaid
---
title: Microsoft.Extensions.DependencyInjection.Abstractions
---
        flowchart TB
        subgraph packages
            Microsoft.Extensions.DependencyInjection.Abstractions

        end
        subgraph projects
NetTilt.Context(((NetTilt.Context)))
NetTilt.Models(((NetTilt.Models)))
NetTilt.WebAPI(((NetTilt.WebAPI)))
NetTilt.Auth(((NetTilt.Auth)))
NetTilt.Logic(((NetTilt.Logic)))
NetTilt.Tests(((NetTilt.Tests)))
    subgraph versions
6.0.0>6.0.0]
7.0.0>7.0.0]
    end
Microsoft.Extensions.DependencyInjection.Abstractions-->6.0.0
6.0.0-->NetTilt.Context
6.0.0-->NetTilt.Models
6.0.0-->NetTilt.WebAPI
6.0.0-->NetTilt.Auth
6.0.0-->NetTilt.Logic
Microsoft.Extensions.DependencyInjection.Abstractions-->7.0.0
7.0.0-->NetTilt.Tests

    end
```

## 8 Microsoft.Extensions.Primitives

```mermaid
---
title: Microsoft.Extensions.Primitives
---
        flowchart TB
        subgraph packages
            Microsoft.Extensions.Primitives

        end
        subgraph projects
NetTilt.Context(((NetTilt.Context)))
NetTilt.Models(((NetTilt.Models)))
NetTilt.WebAPI(((NetTilt.WebAPI)))
NetTilt.Auth(((NetTilt.Auth)))
NetTilt.Logic(((NetTilt.Logic)))
NetTilt.Tests(((NetTilt.Tests)))
    subgraph versions
6.0.0>6.0.0]
7.0.0>7.0.0]
    end
Microsoft.Extensions.Primitives-->6.0.0
6.0.0-->NetTilt.Context
6.0.0-->NetTilt.Models
6.0.0-->NetTilt.WebAPI
6.0.0-->NetTilt.Auth
6.0.0-->NetTilt.Logic
Microsoft.Extensions.Primitives-->7.0.0
7.0.0-->NetTilt.Tests

    end
```

## 9 Microsoft.NETCore.Platforms

```mermaid
---
title: Microsoft.NETCore.Platforms
---
        flowchart TB
        subgraph packages
            Microsoft.NETCore.Platforms

        end
        subgraph projects
NetTilt.GeneratorFromDB(((NetTilt.GeneratorFromDB)))
NetTilt.Models(((NetTilt.Models)))
NetTilt.Context(((NetTilt.Context)))
NetTilt.WebAPI(((NetTilt.WebAPI)))
NetTilt.Auth(((NetTilt.Auth)))
NetTilt.Logic(((NetTilt.Logic)))
NetTilt.Tests(((NetTilt.Tests)))
    subgraph versions
2.1.2>2.1.2]
3.1.0>3.1.0]
    end
Microsoft.NETCore.Platforms-->2.1.2
2.1.2-->NetTilt.GeneratorFromDB
2.1.2-->NetTilt.Models
Microsoft.NETCore.Platforms-->3.1.0
3.1.0-->NetTilt.Context
3.1.0-->NetTilt.WebAPI
3.1.0-->NetTilt.Auth
3.1.0-->NetTilt.Logic
3.1.0-->NetTilt.Tests

    end
```

## 10 NETStandard.Library

```mermaid
---
title: NETStandard.Library
---
        flowchart TB
        subgraph packages
            NETStandard.Library

        end
        subgraph projects
NetTilt.GeneratorFromDB(((NetTilt.GeneratorFromDB)))
NetTilt.Context(((NetTilt.Context)))
NetTilt.Models(((NetTilt.Models)))
NetTilt.Auth(((NetTilt.Auth)))
NetTilt.Logic(((NetTilt.Logic)))
NetTilt.WebAPI(((NetTilt.WebAPI)))
NetTilt.Tests(((NetTilt.Tests)))
    subgraph versions
1.6.1>1.6.1]
2.0.0-preview1-25301-01>2.0.0-preview1-25301-01]
2.0.0>2.0.0]
    end
NETStandard.Library-->1.6.1
1.6.1-->NetTilt.GeneratorFromDB
1.6.1-->NetTilt.Context
1.6.1-->NetTilt.Models
1.6.1-->NetTilt.Auth
1.6.1-->NetTilt.Logic
NETStandard.Library-->2.0.0-preview1-25301-01
2.0.0-preview1-25301-01-->NetTilt.WebAPI
NETStandard.Library-->2.0.0
2.0.0-->NetTilt.Tests

    end
```

## 11 Newtonsoft.Json

```mermaid
---
title: Newtonsoft.Json
---
        flowchart TB
        subgraph packages
            Newtonsoft.Json

        end
        subgraph projects
NetTilt.GeneratorFromDB(((NetTilt.GeneratorFromDB)))
NetTilt.Context(((NetTilt.Context)))
NetTilt.Models(((NetTilt.Models)))
NetTilt.WebAPI(((NetTilt.WebAPI)))
NetTilt.Auth(((NetTilt.Auth)))
NetTilt.Logic(((NetTilt.Logic)))
NetTilt.Tests(((NetTilt.Tests)))
    subgraph versions
12.0.1>12.0.1]
13.0.1>13.0.1]
    end
Newtonsoft.Json-->12.0.1
12.0.1-->NetTilt.GeneratorFromDB
12.0.1-->NetTilt.Context
12.0.1-->NetTilt.Models
Newtonsoft.Json-->13.0.1
13.0.1-->NetTilt.WebAPI
13.0.1-->NetTilt.Auth
13.0.1-->NetTilt.Logic
13.0.1-->NetTilt.Tests

    end
```

## 12 System.Collections.Immutable

```mermaid
---
title: System.Collections.Immutable
---
        flowchart TB
        subgraph packages
            System.Collections.Immutable

        end
        subgraph projects
NetTilt.GeneratorFromDB(((NetTilt.GeneratorFromDB)))
NetTilt.Context(((NetTilt.Context)))
NetTilt.Models(((NetTilt.Models)))
NetTilt.WebAPI(((NetTilt.WebAPI)))
NetTilt.Auth(((NetTilt.Auth)))
NetTilt.Logic(((NetTilt.Logic)))
NetTilt.Tests(((NetTilt.Tests)))
    subgraph versions
5.0.0>5.0.0]
6.0.0>6.0.0]
    end
System.Collections.Immutable-->5.0.0
5.0.0-->NetTilt.GeneratorFromDB
System.Collections.Immutable-->6.0.0
6.0.0-->NetTilt.Context
6.0.0-->NetTilt.Models
6.0.0-->NetTilt.WebAPI
6.0.0-->NetTilt.Auth
6.0.0-->NetTilt.Logic
6.0.0-->NetTilt.Tests

    end
```

## 13 System.Diagnostics.DiagnosticSource

```mermaid
---
title: System.Diagnostics.DiagnosticSource
---
        flowchart TB
        subgraph packages
            System.Diagnostics.DiagnosticSource

        end
        subgraph projects
NetTilt.GeneratorFromDB(((NetTilt.GeneratorFromDB)))
NetTilt.Context(((NetTilt.Context)))
NetTilt.Models(((NetTilt.Models)))
NetTilt.WebAPI(((NetTilt.WebAPI)))
NetTilt.Auth(((NetTilt.Auth)))
NetTilt.Logic(((NetTilt.Logic)))
NetTilt.Tests(((NetTilt.Tests)))
    subgraph versions
4.3.0>4.3.0]
6.0.0>6.0.0]
    end
System.Diagnostics.DiagnosticSource-->4.3.0
4.3.0-->NetTilt.GeneratorFromDB
System.Diagnostics.DiagnosticSource-->6.0.0
6.0.0-->NetTilt.Context
6.0.0-->NetTilt.Models
6.0.0-->NetTilt.WebAPI
6.0.0-->NetTilt.Auth
6.0.0-->NetTilt.Logic
6.0.0-->NetTilt.Tests

    end
```

## 14 System.Runtime.CompilerServices.Unsafe

```mermaid
---
title: System.Runtime.CompilerServices.Unsafe
---
        flowchart TB
        subgraph packages
            System.Runtime.CompilerServices.Unsafe

        end
        subgraph projects
NetTilt.GeneratorFromDB(((NetTilt.GeneratorFromDB)))
NetTilt.Context(((NetTilt.Context)))
NetTilt.Models(((NetTilt.Models)))
NetTilt.WebAPI(((NetTilt.WebAPI)))
NetTilt.Auth(((NetTilt.Auth)))
NetTilt.Logic(((NetTilt.Logic)))
NetTilt.Tests(((NetTilt.Tests)))
    subgraph versions
5.0.0>5.0.0]
6.0.0>6.0.0]
    end
System.Runtime.CompilerServices.Unsafe-->5.0.0
5.0.0-->NetTilt.GeneratorFromDB
System.Runtime.CompilerServices.Unsafe-->6.0.0
6.0.0-->NetTilt.Context
6.0.0-->NetTilt.Models
6.0.0-->NetTilt.WebAPI
6.0.0-->NetTilt.Auth
6.0.0-->NetTilt.Logic
6.0.0-->NetTilt.Tests

    end
```
<small>Generated  by https://www.nuget.org/packages/netpackageanalyzerconsole , version 7.2023.830.921</small>
