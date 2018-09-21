pipeline {
    agent any

    stages {
        stage('Build') {
            steps {
                sh 'mkdir packages && cd packages && nuget restore Integreat.sln'
                sh 'msbuild Integreat.sln /p:AndroidSdkDirectory="/opt/android-sdk/" || true'
                sh 'msbuild Integreat/Integreat.Droid/Integreat.Droid.csproj /p:Configuration=Release /p:AndroidSdkDirectory="/opt/android-sdk/" /target:SignAndroidPackage'
            }
        }
    }
    post {
        always {
            archiveArtifacts artifacts: 'Integreat/Integreat.Droid/bin/Release/*.apk', fingerprint: true
        }
    }
}
