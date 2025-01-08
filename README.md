# UrlShortener
![image](https://github.com/user-attachments/assets/578c0431-a31e-4d7b-b03e-58bc9b1025d3)

## Overview
UrlShortener is a web application designed for shortening URLs and providing efficient URL management. It leverages modern technologies to ensure fast performance, scalability, and robust search capabilities.

## Technology Stack

1. **Frontend**: Vue.js
2. **Backend**: .NET Core
3. **data Storage**: Apache Solr
4. **Caching**: Redis
5. **Server Proxy**: Nginx

## Deployment Setup
The project uses Docker to create a multi-container environment, simplifying the deployment of all components. Docker Compose is used to manage the containers.

### Port Configuration info
| Component   | Service Port | Docker Host Port |
|-------------|--------------|------------------|
| Client      | 2510         | 80               |
| Server      | 7224         | 80               |
| Solr        | 8983         | 8983             |
| Redis       | 6379         | 6379             |

### Prerequisites
Ensure the following are installed on your local system:
- Docker
- Docker Compose

### Instructions to Run the Application

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd <project-root>
   ```

2. **Build the Docker Containers**
   ```bash
   docker-compose build
   ```

3. **Start the Application**
   ```bash
   docker-compose up -d
   ```

4. **Access the Application**
   - Vue.js Client: [http://localhost:2510](http://localhost:2510)
   - Backend Server: [http://localhost:7224](http://localhost:7224)
   - Solr Admin Panel: [http://localhost:8983](http://localhost:8983)


5. **Verify All Components Are Running**
   Run the following command to check the status of the containers:
   ```bash
   docker ps
   ```




 
 
