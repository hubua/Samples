# Mongo
Accessing MongoDB from .NET

## Links & Docs

- DB Tutorials
  - Overview https://www.tutorialspoint.com/mongodb/mongodb_overview.htm
  - Indexes https://docs.mongodb.com/getting-started/csharp/indexes/
  - Transactions with two-phase commits https://docs.mongodb.com/manual/tutorial/perform-two-phase-commits/

- Driver documentation
  - Installation http://mongodb.github.io/mongo-csharp-driver/2.7/getting_started/installation/
  - Quick tour http://mongodb.github.io/mongo-csharp-driver/2.7/getting_started/quick_tour/
  - Implementing Polymorphism http://mongodb.github.io/mongo-csharp-driver/2.7/reference/bson/mapping/polymorphism/

- Helper tools
  - https://robomongo.org/
  - http://www.json-generator.com/

- Use cases
 - https://docs.mongodb.com/ecosystem/use-cases/product-catalog/

## Backup

- https://docs.mongodb.com/manual/core/backups/
- https://www.digitalocean.com/community/tutorials/how-to-back-up-restore-and-migrate-a-mongodb-database-on-ubuntu-14-04

## Security

- https://docs.mongodb.com/manual/tutorial/enable-authentication/
- https://medium.com/mongoaudit/how-to-enable-authentication-on-mongodb-b9e8a924efac

1) create admin user
```
use admin

db.createUser(
  {
    user: "admin",
    pwd: "***",
    roles: [ { role: "userAdminAnyDatabase", db: "admin" }, "readWriteAnyDatabase" ]
  }
)

db.dropUser("admin")
db.changeUserPassword("admin","<new_password>")
```

2) turn authentication on
```
C:\Program Files\MongoDB\Server\4.0\bin\mongod.cfg

security:
  authorization: "enabled"
```

3) restart MongoDB Server
