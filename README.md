
# Sohat Notebook Project

This project is designed to be deployed using **Azure** and **Docker**. Follow the steps below to build, deploy, and run the project.

---

## Prerequisites

1. **Azure Account**  
   Ensure you have an active Azure account.  

2. **Azure CLI**  
   Install the [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli) and log in.  

   ```bash
   az login
   ```

3. **Terraform**  
   Install Terraform to manage the infrastructure setup.  

4. **Docker**  
   Install Docker Desktop on your machine.  

---

## Deployment Instructions

### Step 1: Build the Project  
Before deploying, ensure the project is built successfully to avoid any errors.

---

### Step 2: Configure Infrastructure with Terraform  
Navigate to the `Infrastructure` folder in your terminal and run the following commands:

1. Initialize Terraform:  
   ```bash
   terraform init
   ```

2. Review the infrastructure plan:  
   ```bash
   terraform plan
   ```

3. Apply the infrastructure changes:  
   ```bash
   terraform apply
   ```

---

### Step 3: Build and Run the Docker Image  

1. Build the Docker image:  
   ```bash
   docker build -t sohatnotebook .
   ```

2. Run the Docker container:  
   ```bash
   docker run -p 8080:80 sohatnotebook
   ```

   The application will now be available on **http://localhost:8080**.

---

## Notes

- Make sure Docker is running before building and running the container.
- Any changes to the infrastructure should be managed using Terraform commands from the `Infrastructure` folder.

---

## Enjoy! 😄
