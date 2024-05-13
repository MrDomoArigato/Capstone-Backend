pipeline {
    agent {
        docker { image 'mcr.microsoft.com/dotnet/sdk:8.0'}
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        stage('Test') {
            steps {
                echo 'Testing...'
                sh 'dotnet test'
            }
        }
        stage('Build') {
            steps {
                echo 'Building...'
                sh 'dotnet build'
            }
        }
        stage('Deploy') {
            steps {
                echo 'Deploying....'
            }
        }
    }
}