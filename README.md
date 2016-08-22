# Assessment points

* Original requirements are met by TransmaxTest project.

* Unit tests are in TransmaxTest.UnitTests project, there are 47 test cases in total.

* Solution is published here, on [github](https://github.com/Wombatush/gradescores)

* Tests are run automatically on checkin via a [CI service](https://ci.appveyor.com/project/Wombatush/gradescores)

# Assumptions

* Data layout – according to original requirement data entries seems to be in a format "LAST_NAME, FIRST_NAME, SCORE"; that is why I retrieve score only, and if score value matches for two records I compare records as plain strings.

* Ordering is case sensitive – although it appears that the test data is uppercase, there is still no guarantees that data won’t contain lowercase entries as well. If ordering has to be case insensitive, it is literally one line change;

* Ordering is ordinal – there was no specific requirement on the culture sensitivity, so I decided to use Ordinal rather than invariant culture. Yet again it is a one line change if requirement is different. 