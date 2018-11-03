# Ignore inline messages which lay outside a diff's range of PR
github.dismiss_out_of_range_messages

# PR
if github.pr_title.include? "[WIP]" || github.pr_labels.include?("WIP")
  warn("PR is classed as Work in Progress") 
end

# Warn when there is a big PR
warn("a large PR") if git.lines_of_code > 500

# Warn when PR has no assignees
warn("A pull request must have some assignees") if github.pr_json["assignee"].nil?

resharper_inspectcode.base_path = Dir.pwd
resharper_inspectcode.report "report.xml"