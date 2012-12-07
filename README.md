Nebula
======

This project uses Ruby (with Rake) as a build engine.

You can download ruby from here http://www.ruby-lang.org/en/

After installation you have to install a couple of gems to get started, first open a command prompt and run
gems.bat file to install all necessary gems or you could install them yourself by running following commands.

- gem install rake
- gem install albacore
- gem install version_bumper
- gem install configatron

Under the config directory add development.rb with at least this content

```ruby
# connectionstrings
configatron.connectionstrings.nebula = '#your connectionstring#'
configatron.connectionstrings.nebula_test = '#your connectionstring#'

# Here you can mess with the logging settings
configatron.logging.db.buffersize = 5
configatron.logging.db.minimumlevel = 'WARN'
configatron.logging.file.enabled = true
configatron.logging.file.showsql = true
configatron.logging.file.trace = true
```

Now from your solution folder, run rake from command prompt, this generates all necessary config files

For more task infos run rake -T