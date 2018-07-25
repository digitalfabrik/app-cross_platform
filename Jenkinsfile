pipeline {
    agent any

    stages {
        stage('Build') {
            steps {
                sh 'mkdir packages && cd packages && nuget install ../Integreat/Integreat.Droid/packages.config'
                sh 'msbuild Integreat.sln /p:AndroidSdkDirectory="/opt/android-sdk/"'
            }
        }
    }
}
