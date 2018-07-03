pipeline {
    agent {
        docker { 
            alwaysPull false
            image 'microsoft/dotnet:2.1-sdk'
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

                    if (env.BRANCH_NAME.endsWith("-alpha")) {

                        sh 'dotnet pack ./Oragon.Spring.Core/Oragon.Spring.Core.csproj --configuration Debug /p:PackageVersion="$BRANCH_NAME" --include-source --include-symbols --output ../output-packages'
                        
                        sh 'dotnet pack ./Oragon.Spring.Aop/Oragon.Spring.Aop.csproj --configuration Debug /p:PackageVersion="$BRANCH_NAME" --include-source --include-symbols --output ../output-packages'

                    } else if (env.BRANCH_NAME.endsWith("-beta")) {

                        sh 'dotnet pack ./Oragon.Spring.Core/Oragon.Spring.Core.csproj --configuration Release /p:PackageVersion="$BRANCH_NAME" --output ../output-packages'                        
                        
                        sh 'dotnet pack ./Oragon.Spring.Aop/Oragon.Spring.Aop.csproj --configuration Release /p:PackageVersion="$BRANCH_NAME" --output ../output-packages'                        

                    } else {

                        sh 'dotnet pack ./Oragon.Spring.Core/Oragon.Spring.Core.csproj --configuration Release /p:PackageVersion="$BRANCH_NAME" --output ../output-packages'
                        
                        sh 'dotnet pack ./Oragon.Spring.Aop/Oragon.Spring.Aop.csproj --configuration Release /p:PackageVersion="$BRANCH_NAME" --output ../output-packages'

                    }

                }

            }

        }

        stage('Publish') {

            when { buildingTag() }

            steps {
                
                script {
                    
                    if (env.BRANCH_NAME.endsWith("-alpha")) {
                        
                        withCredentials([usernamePassword(credentialsId: 'myget-oragon', passwordVariable: 'MYGET_KEY', usernameVariable: 'DUMMY' )]) {

                            sh 'dotnet nuget push $(ls ./output-packages/*.nupkg)  -k "$MYGET_KEY" -s https://www.myget.org/F/oragon-alpha/api/v3/index.json'

                        }

                    } else if (env.BRANCH_NAME.endsWith("-beta")) {

                        withCredentials([usernamePassword(credentialsId: 'myget-oragon', passwordVariable: 'MYGET_KEY', usernameVariable: 'DUMMY')]) {

                            sh 'dotnet nuget push $(ls ./output-packages/*.nupkg)  -k "$MYGET_KEY" -s https://www.myget.org/F/oragon-beta/api/v3/index.json'

                        }

                        
                    } else {

                        withCredentials([usernamePassword(credentialsId: 'nuget-luizcarlosfaria', passwordVariable: 'NUGET_KEY', usernameVariable: 'DUMMY')]) {

                            sh 'dotnet nuget push $(ls ./output-packages/*.nupkg)  -k "$NUGET_KEY"'

                        }

                    }                    
				}
            }
        }
    }
}