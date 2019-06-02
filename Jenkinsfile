pipeline {
    agent {
        docker { 
            alwaysPull false
            image 'mcr.microsoft.com/dotnet/core/sdk:3.0'
            reuseNode false
            args '-u root:root'
        }
    }
    stages {
      
        stage('Build') {

            steps {

                // git branch: 'master', credentialsId: 'GITHUB_USERNAME', url: 'https://github.com/Oragon/Oragon.AspNetCore.Hosting.AMQP.git'
                
                echo sh(script: 'env|sort', returnStdout: true)

                sh 'dotnet build ./Oragon.Spring.sln'

            }

        }

        stage('Test') {

            steps {

                // sh 'dotnet test ./Oragon.Spring.Core.Tests/Oragon.Spring.Core.Tests.csproj --configuration Debug --output ../output--core-tests'

                // sh 'dotnet test ./Oragon.Spring.Aop.Tests/Oragon.Spring.Aop.Tests.csproj --configuration Debug --output ../output-aop-tests'

                echo 'Disabled at this time'

            }

        }

        stage('Pack') {

            when { buildingTag() }

            steps {

                script{

                    def projetcs = [
                        './Oragon.Spring.Core/Oragon.Spring.Core.csproj', 
                        './Oragon.Spring.Aop/Oragon.Spring.Aop.csproj',
                        './Oragon.Spring.Extensions.DependencyInjection/Oragon.Spring.Extensions.DependencyInjection.csproj'
                    ]

                    if (env.BRANCH_NAME.endsWith("-alpha")) {

                        for (int i = 0; i < projetcs.size(); ++i) {
                            sh "dotnet pack ${projetcs[i]} --configuration Debug /p:PackageVersion=${BRANCH_NAME} --include-source --include-symbols --output ../output-packages"
                        }

                    } else if (env.BRANCH_NAME.endsWith("-beta")) {

                        for (int i = 0; i < projetcs.size(); ++i) {
                            sh "dotnet pack ${projetcs[i]} --configuration Release /p:PackageVersion=${BRANCH_NAME} --output ../output-packages"                        
                        }

                    } else {

                        for (int i = 0; i < projetcs.size(); ++i) {
                            sh "dotnet pack ${projetcs[i]} --configuration Release /p:PackageVersion=${BRANCH_NAME} --output ../output-packages"                        
                        }

                    }

                }

            }

        }

        stage('Publish') {

            when { buildingTag() }

            steps {
                
                script {
                    
                    def publishOnNuGet = ( env.BRANCH_NAME.endsWith("-alpha") == false );

                    withCredentials([usernamePassword(credentialsId: 'myget-oragon', passwordVariable: 'MYGET_KEY', usernameVariable: 'DUMMY' )]) {
                        
                        sh 'for pkg in ../output-packages/*.nupkg ; do dotnet nuget push "$pkg" -k "$MYGET_KEY" -s https://www.myget.org/F/oragon/api/v3/index.json ; done'

                    }

                    if (publishOnNuGet) {
                        
                        withCredentials([usernamePassword(credentialsId: 'nuget-luizcarlosfaria', passwordVariable: 'NUGET_KEY', usernameVariable: 'DUMMY')]) {

                            sh 'for pkg in ../output-packages/*.nupkg ; do dotnet nuget push "$pkg" -k "$NUGET_KEY" -s https://api.nuget.org/v3/index.json ; done'

                        }

                    }                    
				}
            }
        }
    }
}