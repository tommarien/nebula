require 'rake/clean'
require 'albacore'
require 'version_bumper'
require 'configatron'
require 'erb'

#global variables
@build_folder = File.expand_path("./build");
@src_folder = File.expand_path("./src");

#require all rb files from build folder
Dir.glob("#{@build_folder}/*.rb") do |f|
	require f
end

#include all generated files in clean
Dir.glob("**/*.*.template") do |filename|
	filename[".template"] = ""
	CLEAN.include(filename)
end

@env_root_path = File.expand_path('.')
@env_sourcepath = File.join("#{@env_root_path}", "src")
@env_cfg_path = File.join("#{@env_root_path}", "config")

@env_packagespath = File.expand_path('./packages')
@env_templatespath = File.expand_path('./templates')
@env_sharedassemblyinfo = File.join("#{@env_sourcepath}", "SharedAssemblyInfo.cs")
@env_projectname = "Nebula"
@env_buildconfigname = "Release"



def env_buildversion
  bumper_version.to_s
end

def env_projectfullname
  "#{@env_projectname}-v#{env_buildversion}-#{@env_buildconfigname}"
end

def env_buildfolderpath
  "Builds/#{env_projectfullname}/"
end

task :default => "env:setup"

namespace :env do

	def init(env=nil)
		root = defined?(Rails) ? ::Rails.root : FileUtils.pwd
        base_dir = File.expand_path(File.join(root, 'config'))
		if env.nil?
			env = defined?(Rails) ? ::Rails.env : 'development'
		end
		
		config_files = []

		config_files << File.join(base_dir, 'defaults.rb')
		config_files << File.join(base_dir, "#{env}.rb")

		env_dir = File.join(base_dir, env)
		config_files << File.join(env_dir, 'defaults.rb')

		Dir.glob(File.join(env_dir, '*.rb')).sort.each do |f|
			config_files << f
		end

		config_files.collect! {|config| File.expand_path(config)}.uniq!

		config_files.each do |config|
			if File.exists?(config)
			  # puts "Configuration: #{config}"
			  require config
			end
		end
	end
	
	desc "Setup your environment"
	task :setup, [:env] do |t, args|
		init(args.env)
		
		Dir.glob("**/*.*.template") do |filename|
			erb = ERB.new(File.read(filename))
			filename[".template"] = ""
			File.open(filename, 'w') { |f| f.write erb.result() }
		end
		
		puts "Configured application for environment"
	end
end

namespace :build do
	desc "build Nebula in Debug configuration"
	msbuild :debug do |msb|
		msb.properties = {:configuration => :Debug}
		msb.targets = [:Clean, :Build]
		msb.solution = configatron.product.solution
	end
  
	desc "build Nebula in Release configuration"
	msbuild :release => "version:assemblyinfo" do |msb|
		msb.properties = {:configuration => :Release}
		msb.targets = [:Clean, :Build]
		msb.solution = configatron.product.solution
	end
end

namespace :db do

	# figure out the location of fluentmigrator console
	def fluentmigrator_command
			fluentdirectory = Dir.glob('packages/FluentMigrator.Tools.*/tools/AnyCPU/40')
			
			if fluentdirectory.empty? then
				raise "FluentMigrator.Tools package is missing, build your solution first !"
			else 
				"#{fluentdirectory.last}/Migrate.exe"
			end
	end
	
	fluentmigrator_provider = 'SqlServer2008'
	
	# Todo: Take into account that we could have build in release/debug mode
	def fluentmigrator_target
		target = './src/Nebula.Migrations/bin/Debug/Nebula.Migrations.dll'
		if File.exists?(target)
			target
		else
			raise "Migration assembly is missing, looked in #{target}"
		end
	end
	

	desc "runs all necessary migrations"
	fluentmigrator :migrate, :connection do |migrator, args|
		migrator.command = fluentmigrator_command
		migrator.provider = fluentmigrator_provider
		migrator.target = fluentmigrator_target
		migrator.connection = args[:connection]
		migrator.verbose = true
	end
	
	desc "rollbacks all migrations"
	fluentmigrator :rollback, :connection do |migrator, args|
		migrator.command = fluentmigrator_command
		migrator.provider = fluentmigrator_provider
		migrator.target = fluentmigrator_target
		migrator.connection = args[:connection]
		migrator.task = "rollback:all"
		migrator.verbose = true
	end
	
	desc "WARN: rollbacks and reruns all migrations"
	task :reset, [:connection] do |t, args|
		Rake::Task["db:rollback"].invoke(args.connection)
		Rake::Task["db:migrate"].invoke(args.connection)
	end
end