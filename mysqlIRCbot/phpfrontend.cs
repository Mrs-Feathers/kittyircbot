/*

<?php
$connection = mysql_connect('host', 'username', 'password');
    // Check MYSQL connection
    if ($connection) {
    $select = mysql_select_db("irc", $connection);
        if ($select) {
        echo "<b>Currently connected users on IRC";
        $query = "select user from irc";
        $result = mysql_query($query);
        $numrows = mysql_num_rows($result);
        $i = 0;
            while ($i < $numrows) {
            echo "<br /> " . mysql_result($result, $i);
            $i++;
            }
        }
        else echo "No connection to MYSQL server!";
    }
?>
	
	*/
