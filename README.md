## Web app with queues using RabbitMQ

To run:
1. When running for the first time, uncomment ```context.FillDb();``` in backend/WorkerService/Program.cs, to fill the database with initial info.
2. Run WorkerService and API from the backend folder.
3. install Node.js
4. in the frontend folder run:
```
ng serve
```