require 'albacore'
require 'version_bumper'

@env_sourcepath = "./src/"
@env_projectname = "Nebula"
@env_buildconfigname = "Release"

@connectionstring = "Server=.\NOCTO;Integrated Security=true;Initial Catalog=Nebula"

def env_buildversion
  bumper_version.to_s
end

def env_projectfullname
  "#{@env_projectname}-v#{env_buildversion}-#{@env_buildconfigname}"
end

def env_buildfolderpath
  "Builds/#{env_projectfullname}/"
end

namespace :version do
	desc "writes shared assembly info."
	assemblyinfo :assemblyinfo do |asm|
	  asm.output_file = "#{@env_sourcepath}SharedAssemblyInfo.cs"
	  asm.version = env_buildversion
	  asm.file_version = env_buildversion  
	end
end

namespace :build do
	desc "build the debug version of the solution."
	msbuild :debug => "version:assemblyinfo" do |msb|
		msb.properties = {:configuration => :Debug}
		msb.targets = [:Clean, :Build]
		msb.solution = "Nebula.sln"
	end
  
	desc "build the release version of the solution."
	msbuild :release => "version:assemblyinfo" do |msb|
		msb.properties = {:configuration => :Release}
		msb.targets = [:Clean, :Build]
		msb.solution = "Nebula.sln"
	end
end

task :default => "build:debug"

namespace :db do
	desc "runs all necessary migrations"
	fluentmigrator :migrate, :connection do |migrator, args|
		migrator.command = 'packages/FluentMigrator.Tools.1.0.2.0/tools/AnyCPU/40/Migrate.exe'
		migrator.provider = 'SqlServer2008'
		migrator.target = './src/Nebula.Migrations/bin/Debug/Nebula.Migrations.dll'
		migrator.connection = args[:connection]
		migrator.verbose = true
	end
	desc "rollbacks all migrations"
	fluentmigrator :rollback, :connection do |migrator, args|
		migrator.command = 'packages/FluentMigrator.Tools.1.0.2.0/tools/AnyCPU/40/Migrate.exe'
		migrator.provider = 'SqlServer2008'
		migrator.target = './src/Nebula.Migrations/bin/Debug/Nebula.Migrations.dll'
		migrator.connection = args[:connection]
		migrator.task = "rollback:all"
		migrator.verbose = true
	end
end