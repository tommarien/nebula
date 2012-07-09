Nebula
======

This project uses Ruby (with Rake) as a build engine.

You can download ruby from here http://www.ruby-lang.org/en/

After installation you have to install a couple of gems to get started, first open a command prompt

- gem install rake
- gem install albacore
- gem install version_bumper
- gem install configatron

Under the config directory add development.rb with at least this content

# connectionstrings
configatron.connectionstrings.nebula = #your connectionstring#
configatron.connectionstrings.nebula_test = #your connectionstring#

Now from your solution folder, run rake from command prompt, this generates all necessary config files

For more task infos run rake -T