pipeline {
    agent any

    environment {
        DOTNET_ROOT = tool name: 'dotnet-sdk', type: 'com.cloudbees.jenkins.plugins.customtools.CustomTool' // Adjust tool name as per Jenkins setup
    }

    stages {
        stage('Checkout') {
            steps {
                // Checkout the code from your Git repository
                checkout scm
            }
        }

        stage('Restore Dependencies') {
            steps {
                // Restore .NET dependencies
                sh 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                // Build the .NET project
                sh 'dotnet build --configuration Release'
            }
        }

        stage('Test') {
            steps {
                // Run unit tests
                sh 'dotnet test --configuration Release'
            }
        }

        stage('Publish') {
            steps {
                // Publish the app for deployment
                sh 'dotnet publish --configuration Release --output publish/'
            }
        }

        stage('Archive Artifacts') {
            steps {
                // Archive the build artifacts (optional)
                archiveArtifacts artifacts: 'publish/**', allowEmptyArchive: true
            }
        }

        stage('Deploy') {
            when {
                branch 'main' // Only deploy for the main branch
            }
            steps {
                // Deploy the application (customize as needed)
                echo 'Deploying the application...'
                // Example: Copy files to a deployment server
                sh 'scp -r publish/* user@your-server:/path/to/deployment'
            }
        }
    }

    post {
        success {
            echo 'Pipeline completed successfully.'
        }
        failure {
            echo 'Pipeline failed.'
        }
    }
}
