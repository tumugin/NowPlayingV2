# Ignore inline messages which lay outside a diff's range of PR
github.dismiss_out_of_range_messages

# PR
if github.pr_title.include? "[WIP]" || github.pr_labels.include?("WIP")
  warn("PR is classed as Work in Progress") 
end

# Warn when there is a big PR
warn("a large PR") if git.lines_of_code > 500

# Warn when PR has no milestone
warn("A pull request must have a milestone set") if github.pr_json["milestone"].nil?

# Warn when PR has no assignees
warn("A pull request must have some assignees") if github.pr_json["assignee"].nil?

# Android Lint
android_lint.gradle_task = "app:lint"
android_lint.report_file = "app/build/reports/lint-results.xml"
android_lint.filtering = true
android_lint.lint(inline_mode: true)

# ktlint
checkstyle_format.base_path = Dir.pwd
checkstyle_format.report 'app/build/reports/ktlint/ktlint-main.xml'