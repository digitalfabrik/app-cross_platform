pipeline {
    agent any

    stages {
        stage('Build') {
            steps {
                sh 'mkdir packages && cd packages && nuget install ../Integreat/Integreat.Droid/packages.config'
                sh 'msbuild Integreat.sln /p:AndroidSdkDirectory="/opt/android-sdk/" || true'
                sh 'msbuild Integreat.sln /p:AndroidSdkDirectory="/opt/android-sdk/" /t:Build;CopyFilesToOutputDirectory'
            }
        }
    }
    post {
        always {
            archiveArtifacts artifacts: 'Integreat/Integreat.Droid/bin/Debug/*.apk', fingerprint: true
        }
    }
}
