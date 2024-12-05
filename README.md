To Run the project You should build it first to make sure there are no any errors
YOU MUST HAVE AZURE ACCOUNT
Run "az login" Command from your terminal and sign in to azure 
Run terraform commands from termanal opened in the "Infrastucture" Folder
      1- terraform init
      2- terraform plan
      3- terraform apply
You should have docker installed on your machine 
Run docker and then Run the command " docker build -t sohatnotebook . " -> Make an image from your project
Run "docker run -p 8080:80 sohatnotebook" -> specify your port and run this command to run the docker container.
Enjoy :D
