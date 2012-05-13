require 'version_bumper'

configatron.product.name = 'Nebula'
configatron.product.solution = configatron.product.name + '.sln'
configatron.product.version = Configatron::Dynamic.new {bumper_version.to_s}
configatron.product.configuration = "Debug"
configatron.product.fullname = Configatron::Dynamic.new {"#{configatron.product.name}-v#{configatron.product.version}-#{configatron.product.configuration}"}